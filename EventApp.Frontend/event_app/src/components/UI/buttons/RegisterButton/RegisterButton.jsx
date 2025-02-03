import React from "react";
import classes from "./RegisterButton.module.css";

const RegisterButton = ({type="button", onClick=()=>{}, ...props}) => {
    return (
        <button {...props} type={type} onClick={onClick} className={classes.registerButton}>
            Зарегистрироваться
        </button>
    );
}

export default RegisterButton;