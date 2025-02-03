import React from "react";
import classes from "./AuthenticatedHeaderButtons.module.css"

const LogoutButton = ({type="button", onClick=()=>{}, ...props}) => {
    return (
        <button type={type} onClick={onClick} {...props} className={classes.headerButton}>
            Выйти
        </button>
    );
}

export default LogoutButton;