import React from "react";
import classes from "./AuthenticatedHeaderButtons.module.css"

const CreateEventButton = ({type="button", onClick=()=>{}, ...props}) => {
    return (
        <button type={type} onClick={onClick} {...props} className={classes.headerButton}>
            Добавить событие
        </button>
    );
}

export default CreateEventButton;