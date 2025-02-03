import React from "react";
import classes from "./LoginButton.module.css"

const LoginButton = ({type="button", onClick=()=>{}, ...props}) => {
    return (
        <button {...props} type={type} onClick={onClick} className={classes.loginButton}>
            Войти
        </button>
    );
}

export default LoginButton;