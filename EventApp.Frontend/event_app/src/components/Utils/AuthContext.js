import {useState, useEffect, createContext, useContext} from "react"; 
import baseUrl from "./baseUrl";

const Context = createContext();

export default function AuthContext({children, setIsAdmin, setIsAuthenticated, setCurrentUser}){
    const defaultState = {accessToken: '', refreshToken: ''}

    const [tokens, setTokens] = useState(defaultState);

    const UpdateAccessToken = () =>{
        const oldAccessToken = localStorage.getItem("AccessToken");
        const oldRefreshToken = localStorage.getItem("RefreshToken");
        const isAdmin = localStorage.getItem("isAdmin") === "true";

        if(oldAccessToken && oldRefreshToken){
            const body = {
                refreshToken: oldRefreshToken,
                accessToken: oldAccessToken
            }
            fetch(`${baseUrl}api/Token/Refresh`, {
                body: JSON.stringify(body),
                method: "PUT",
                headers: {"Content-type": "application/json"}
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status)
                }
                return response.json()
            })
            .then((jsonObject) => {
                setTokens({accessToken: jsonObject.accessToken, refreshToken: jsonObject.refreshToken});
                localStorage.setItem("AccessToken", jsonObject.accessToken);
                localStorage.setItem("RefreshToken", jsonObject.refreshToken);
                if(isAdmin){
                    setIsAuthenticated(true);
                    setIsAdmin(true);
                }
                else{
                    fetch(`${baseUrl}api/Participant`, {
                        method: "GET",
                        headers: {"Authorization": `Bearer ${jsonObject.accessToken}`}
                    })
                    .then((response) => {
                        if(!response.ok){
                            throw new Error(response.status)
                        }
                        return response.json();
                    })
                    .then((jsonObject) => {
                        const user = {
                            id: jsonObject.id,
                            name: jsonObject.name,
                            surname: jsonObject.surname,
                            email: jsonObject.email,
                            birthDate: jsonObject.birthDate,
                            events: jsonObject.events
                        }
                        setCurrentUser(user);
                        setIsAuthenticated(true);
                        setIsAdmin(false);
                    })
                    .catch((e) => {
                        console.error(e.message);
                    })
                }
                console.log(jsonObject);
            })
            .catch((e) => {
                if(e.message == 406){
                    setTokens(defaultState);
                    localStorage.removeItem("AccessToken");
                    localStorage.removeItem("RefreshToken");
                }
                setIsAdmin(false);
                setIsAuthenticated(false);
            })
        }
    }

    useEffect(() => {
        UpdateAccessToken();
        const interval = setInterval(UpdateAccessToken, 15*60*1000)
        return () => {
            clearInterval(interval);
        }
    }, [])

    return (<Context.Provider value={{accessToken: tokens.accessToken, setTokens}}>
        {children}
    </Context.Provider>)
}

export const useAccessToken = () => useContext(Context).accessToken;
export const useSetTokens = () => useContext(Context).setTokens;