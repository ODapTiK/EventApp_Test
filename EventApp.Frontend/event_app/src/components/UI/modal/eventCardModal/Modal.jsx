import React from "react";
import classes from "./Modal.module.css"

const Modal = ({children, modalVisibility, setModalVisibility}) => {

    const rootClasses = [classes.modal];

    if(modalVisibility){
        rootClasses.push(classes.active);
    }

    return (
        <div className={rootClasses.join(' ')} onClick={() => setModalVisibility(false)}>
            <div className={classes.modalContent} onClick={(e) => e.stopPropagation()}>
                {children}
            </div>
        </div>
    );
}

export default Modal;