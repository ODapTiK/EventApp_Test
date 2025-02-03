import React from "react";
import classes from "./CancelButton.module.css";

const CancelButton = ({type="button", onClick=()=>{}, children, ...props}) => {
    return (
        <button {...props} type={type} onClick={onClick} className={classes.cancelButton}>
            {children}
        </button>
    );
}

export default CancelButton;