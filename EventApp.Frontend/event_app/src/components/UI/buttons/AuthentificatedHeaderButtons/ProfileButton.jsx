import React from "react";
import classes from "./AuthenticatedHeaderButtons.module.css"

const ProfileButton = ({type="button", onClick=()=>{}, ...props}) => {
    return (
        <button type={type} onClick={onClick} {...props} className={classes.headerButton}>
            Личный кабинет
        </button>
    );
}

export default ProfileButton;