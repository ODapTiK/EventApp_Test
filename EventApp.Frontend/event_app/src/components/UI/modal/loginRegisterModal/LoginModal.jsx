import React, {useState} from "react";
import classes from "./LoginModal.module.css"

const LoginModal = ({children, modalVisibility, login, setModalVisibility, setIsLoginModal}) => {

    const [isAdminLogin, setIsAdminLogin] = useState(false); 

    const handleToggleForm = () => {
        setIsLoginModal(!login);
        setIsAdminLogin(false); 
    };

    const handleAdminLoginToggle = () => {
        setIsAdminLogin(true);
    };

    const handleParticipantLoginToggle = () => {
        setIsAdminLogin(false);
    };

    const rootClasses = [classes.modal];

    if(modalVisibility){
        rootClasses.push(classes.active);
    }

    return (
        <div className={rootClasses.join(' ')} onClick={() => setModalVisibility(false)}>
            <div className={classes.modalContent} onClick={(e) => e.stopPropagation()}>
            {login ? (
                    <div>
                        {React.cloneElement(children[1], { 
                            isAdminLogin, 
                            ...children[1].props 
                        })}  
                        <p className={classes.modalText}>Нет аккаунта? 
                            <span onClick={handleToggleForm} className={classes.modalLink}> Регистрация</span>
                        </p>
                        {isAdminLogin ? (
                            <p className={classes.modalText}>Нет прав администратора? 
                                <span onClick={handleParticipantLoginToggle} className={classes.modalLink}> Войти как участник</span>
                            </p>
                        ) : (
                            <p className={classes.modalText}>Есть права администратора? 
                                <span onClick={handleAdminLoginToggle} className={classes.modalLink}> Войти как администратор</span>
                            </p>
                        )}
                    </div>
                ) : (
                    <div>
                        {children[0]} 
                        <p className={classes.modalText}>Уже есть аккаунт? 
                            <span onClick={handleToggleForm} className={classes.modalLink}> Войти!</span>
                        </p>
                    </div>
                )}
            </div>
        </div>
    );
}

export default LoginModal;