import React, {useState} from "react";
import Input from "../../input/Input";
import CancelButton from "../../buttons/cancelButton/CancelButton";
import SubmitButton from "../../buttons/submitButton/SubmitButton";
import csbstyle from "../../../../styles/CancelSubmitButtonStyle.module.css"
import classes from "./LoginForm.module.css"
import { useSetTokens } from "../../../Utils/AuthContext";

const LoginForm = ({setModalVisibility, isAdminLogin, setCurrentUser, setIsAuthenticated, setIsAdmin }) => {

    const setTokens = useSetTokens();
    const defaultState = {Email: '', Password: ''};
    const [loginData, setLoginData] = useState(defaultState);

    const Login = (e) => {
        e.preventDefault();
        const body = {
            email: loginData.Email,
            password: loginData.Password
        }
        if(isAdminLogin){
            fetch("https://localhost:7164/api/Admin/AuthAdmin/Auth", {
                body: JSON.stringify(body),
                method: "PUT",
                headers: {"Content-type": "application/json"}
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status);
                }
                return response.json();
            })
            .then((jsonObject) => {
                setTokens(jsonObject);
                localStorage.setItem("isAdmin", "true");
                localStorage.setItem("AccessToken", jsonObject.accessToken);
                localStorage.setItem("RefreshToken", jsonObject.refreshToken);
                setModalVisibility(false);
                setIsAuthenticated(true);
                setIsAdmin(true);
            })
            .catch((e) => {
                console.error(e.message);
            })
        }
        else{
            fetch("https://localhost:7164/api/Participant/Auth", {
                body: JSON.stringify(body),
                method: "PUT",
                headers: {"Content-type": "application/json"}
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status);
                }
                return response.json();
            })
            .then((jsonObject) => {
                localStorage.setItem("AccessToken", jsonObject.accessToken);
                localStorage.setItem("RefreshToken", jsonObject.refreshToken);
                localStorage.setItem("isAdmin", "false");
                setTokens(jsonObject);
                fetch("https://localhost:7164/api/Participant", {
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
                    setModalVisibility(false);
                    setIsAuthenticated(true);
                    setIsAdmin(false);
                })
                .catch((e) => {
                    console.error(e.message);
                })
            })
            .catch((e) => {
                console.error(e.message);
            })
        }
        setLoginData(defaultState);
    }
  
    const ChangeField = (e, fieldName) => {
        setLoginData({...loginData, [fieldName]: e.target.value});
    }

    return (
        <form className={classes.form} onSubmit={Login}>
            <Input value={loginData.Email} onChange={e => ChangeField(e, "Email")} type="email" placeholder="Электронная почта"/>
            <Input value={loginData.Password} onChange={e => ChangeField(e, "Password")} type="password" placeholder="Пароль"/>
            <div className={csbstyle.block}>
                <CancelButton onClick={() => setModalVisibility(false)}>Отмена</CancelButton>
                <SubmitButton type="submit">Войти</SubmitButton>
            </div>
        </form>
    );
}

export default LoginForm; 