import React from "react";
import classes from "./Header.module.css"
import LogoutButton from "../buttons/AuthentificatedHeaderButtons/LogoutButton";
import ProfileButton from "../buttons/AuthentificatedHeaderButtons/ProfileButton";
import RegisterButton from "../buttons/RegisterButton/RegisterButton";
import LoginButton from "../buttons/LoginButton/LoginButton";
import CreateEventButton from "../buttons/AuthentificatedHeaderButtons/CreateEventButton";
import { Link, useNavigate } from "react-router-dom";
import { useSetTokens } from "../../Utils/AuthContext";

const Header = ({isAuthenticated, isAdmin=false, user={}, setIsLoginModal, 
    setLoginModalVisibility, setIsAuthentificated, setIsAdmin, setCreateEventModalVisibility, setIsCreatingUser, setIsCreatingEvent, setCurrentUser}) => {
    
    const navigate = useNavigate();

    const setTokens = useSetTokens();

    const defaultUser = {id: '', name: "Гость", surname: '', email: '', birthDate: '', events: []};

    const loginOnClick = () => {
        setIsLoginModal(true);
        setLoginModalVisibility(true);
    }
    
    const registerOnClick = () => {
        setIsLoginModal(false);
        setLoginModalVisibility(true);
        setIsCreatingUser(true);
    }

    const handleLogout = () => {
        localStorage.removeItem("AccessToken");
        localStorage.removeItem("RefreshToken");
        setTokens({accessToken: '', refreshToken: ''});
        setCurrentUser(defaultUser);
        setIsAuthentificated(false);
        setIsAdmin(false);
        navigate("/Events")
    }

    const handleProfileOnClick = () => {
        navigate("/Profile")
    }

    const handleCreate = () => {
        setCreateEventModalVisibility(true);
        setIsCreatingEvent(true);
    }

    return (
      <header className={classes.header}>
        <Link to="/Events" className={classes.siteTitle}>Поиск приключений</Link>
        <div className={classes.authButtons}>
            {isAuthenticated ? (<>
                {isAdmin ? (
                    <div className={classes.userInfo}>
                        <span className={classes.userName}>Администратор</span>
                        <CreateEventButton onClick={handleCreate}/>
                        <LogoutButton onClick={handleLogout}/>
                    </div>
                ) : (
                    <div className={classes.userInfo}>
                        <span className={classes.userName}>{`${user.name} ${user.surname}`}</span>
                        <ProfileButton onClick={handleProfileOnClick}/>
                        <LogoutButton onClick={handleLogout}/>
                    </div>
                )}
                </>  
                ) : (
                <div className={classes.userInfo}>
                    <LoginButton onClick={loginOnClick}/>
                    <RegisterButton onClick={registerOnClick}/>
                </div>
            )}
        </div>
      </header>  
    );
}

export default Header;