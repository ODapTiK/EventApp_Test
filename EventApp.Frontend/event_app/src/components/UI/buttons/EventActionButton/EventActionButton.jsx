import React from "react";
import classes from "./EventActionButton.module.css"

const EventActionButton = ({children, type="button", onClick=()=>{}, ...props}) => {
    return (
        <button type={type} onClick={onClick} {...props} className={classes.participateButton}>
            {children}
        </button>
    );
}

export default EventActionButton;