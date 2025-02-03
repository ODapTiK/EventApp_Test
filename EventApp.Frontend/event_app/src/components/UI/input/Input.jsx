import React from "react";
import classes from "./Input.module.css";

const Input = (props) => {
    return (
        <input className={classes.inputField} {...props}/>
    );
}

export default Input;