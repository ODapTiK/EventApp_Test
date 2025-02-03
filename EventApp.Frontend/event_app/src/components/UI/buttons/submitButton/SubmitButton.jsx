import React from "react";
import classes from './SubmitButton.module.css';

const SubmitButton = ({type="button", onClick=()=>{}, children, ...props}) => {
    return (
        <button {...props} type={type} onClick={onClick} className={classes.submitButton}>
            {children}
        </button>
    );
}

export default SubmitButton;