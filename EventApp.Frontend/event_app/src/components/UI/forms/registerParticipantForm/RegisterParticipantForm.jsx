import React, {useState} from "react";
import classes from "./RegisterParticipantForm.module.css"
import csbstyle from "../../../../styles/CancelSubmitButtonStyle.module.css"
import Input from "../../input/Input";
import SubmitButton from "../../buttons/submitButton/SubmitButton";
import CancelButton from "../../buttons/cancelButton/CancelButton";
import baseUrl from "../../../Utils/baseUrl";
import { useAccessToken } from "../../../Utils/AuthContext";

const RegisterParticipantForm = ({setModalVisibility, isCreating=true}) => {

    const accessToken = useAccessToken();
    const defaultState = {Name: '', Surname: '', Email: '', Password: '', BirthDate: ''};
    const [participantInfo, setParticipantInfo] = useState(defaultState);

    const RegisterParticipant = (e) => {
        e.preventDefault();
        console.log(participantInfo);
        if(isCreating){
            const body = {
                name: participantInfo.Name,
                surname: participantInfo.Surname,
                email: participantInfo.Email,
                password: participantInfo.Password,
                birthDate: participantInfo.BirthDate
            }
            fetch(`${baseUrl}api/Participant`, {
                body: JSON.stringify(body),
                method: "POST",
                headers:  {"Content-type": "application/json"}
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status);
                }
                return response.json()
            })
            .catch((e) => {
                console.error(e.message);
            })
        }
        else{
            const body = {
                name: participantInfo.Name,
                surname: participantInfo.Surname,
                birthDate: participantInfo.BirthDate
            }
            fetch(`${baseUrl}api/Participant`, {
                body: JSON.stringify(body),
                method: "PUT",
                headers:  {
                    "Content-type": "application/json",
                    "Authorization": `Bearer ${accessToken}`
                }
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status);
                }
                return response.json()
            })
            .catch((e) => {
                console.error(e.message);
            })
        }
        setParticipantInfo(defaultState);
    }
  
    const ChangeField = (e, fieldName) => {
        setParticipantInfo({...participantInfo, [fieldName]: e.target.value});
    }

    return (
        <form className={classes.form} onSubmit={RegisterParticipant}>
            {isCreating ? (<>       
                <Input value={participantInfo.Name} onChange={e => ChangeField(e, "Name")} type="text" placeholder="Имя"/>
                <Input value={participantInfo.Surname} onChange={e => ChangeField(e, "Surname")} type="text" placeholder="Фамилия"/>
                <Input value={participantInfo.BirthDate} onChange={e => ChangeField(e, "BirthDate")} type="date" placeholder="Дата рождения"/>
                <Input value={participantInfo.Email} onChange={e => ChangeField(e, "Email")} type="text" placeholder="Электронная почта"/>
                <Input value={participantInfo.Password} onChange={e => ChangeField(e, "Password")} type="password" placeholder="Пароль"/>
            </>) : (<>
                <Input value={participantInfo.Name} onChange={e => ChangeField(e, "Name")} type="text" placeholder="Имя"/>
                <Input value={participantInfo.Surname} onChange={e => ChangeField(e, "Surname")} type="text" placeholder="Фамилия"/>
                <Input value={participantInfo.BirthDate} onChange={e => ChangeField(e, "BirthDate")} type="date" placeholder="Дата рождения"/>
            </>)}

            <div className={csbstyle.block}>
                <CancelButton onClick={() => setModalVisibility(false)}>Отмена</CancelButton>
                <SubmitButton type="submit">{isCreating ? "Вперёд!" : "Обновить"}</SubmitButton>
            </div>
      </form>
    );
}

export default RegisterParticipantForm;