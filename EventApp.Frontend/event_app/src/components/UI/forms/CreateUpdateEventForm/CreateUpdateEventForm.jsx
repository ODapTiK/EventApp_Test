import React, { useState } from "react";
import Input from "../../input/Input";
import baseUrl from "../../../Utils/baseUrl";
import CancelButton from "../../buttons/cancelButton/CancelButton";
import SubmitButton from "../../buttons/submitButton/SubmitButton";
import classes from "./CreateUpdateEventForm.module.css";
import { useAccessToken } from "../../../Utils/AuthContext";

const CreateUpdateEventForm = ({isCreating=true, updateId='', setModalVisibility, setEvents}) => {

    const accessToken = useAccessToken();
    const toBase64 = (img, callback) =>{
        var reader = new FileReader();
        reader.onloadend = function(){
            callback(reader.result)
        }
        reader.readAsDataURL(img);
    }

    const defaultFormState = {
        title: '',
        description: '',
        venue: '',
        dateTime: '',
        category: '',
        maxParticipants: '',
        image: '',}
    
    const [eventDetails, setEventDetails] = useState(defaultFormState);

    const ChangeField = (e, fieldName) => {
        setEventDetails({...eventDetails, [fieldName]: e.target.value});
    }
    
    const handleFileChange = (e) => {
        toBase64(e.target.files[0], function(base64string){
            setEventDetails(prevDetails => ({
                ...prevDetails,
                image: base64string,
            })); 
        })
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if(isCreating){
            const body = {
                title: eventDetails.title,
                description: eventDetails.description,
                eventDateTime: eventDetails.dateTime,
                venue: eventDetails.venue,
                category: eventDetails.category,
                maxParticipants: eventDetails.maxParticipants,
                image: eventDetails.image
            }
            fetch(`${baseUrl}api/Admin/CreateEvent/Event`, {
                body: JSON.stringify(body),
                method: "POST",
                headers: {
                    "Content-type": "application/json",
                    "Authorization": `Bearer ${accessToken}`
                }
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status)
                }
                return response.text();
            })
            .then((id) => {
                console.log(id);
                setEvents((events) => [...events, {...body, id, participants: []}])
                setEventDetails(defaultFormState);
            })
            .catch((e) => {
                console.error(e.message);
            })
        }
        else{
            const body = {
                id: updateId,
                title: eventDetails.title,
                description: eventDetails.description,
                eventDateTime: eventDetails.dateTime,
                venue: eventDetails.venue,
                category: eventDetails.category,
                maxParticipants: eventDetails.maxParticipants,
                image: eventDetails.image
            }
            fetch(`${baseUrl}api/Admin/UpdateEvent/Event`, {
                body: JSON.stringify(body),
                method: "PUT",
                headers: {
                    "Content-type": "application/json",
                    "Authorization": `Bearer ${accessToken}`
                }
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status)
                }
            })
            .then(() => {
                setEvents((events) => events.map((e) => e.id !== updateId ? e : {...e, ...body}));
                setEventDetails(defaultFormState);
            })
            .catch((e) => {
                console.error(e.message);
            })
        }
        console.log(eventDetails);
        setModalVisibility(false);
    };

    const handleCancel = () => {
        setEventDetails(defaultFormState);
        setModalVisibility(false);
    };

    return (
        <form onSubmit={handleSubmit} className={classes.form}>
            <Input value={eventDetails.title} onChange={e => ChangeField(e, "title")} type="text" placeholder="Название события"/>
            <Input value={eventDetails.description} onChange={e => ChangeField(e, "description")} type="text" placeholder="Описание события"/>
            <Input value={eventDetails.category} onChange={e => ChangeField(e, "category")} type="text" placeholder="Категория события"/>
            <Input value={eventDetails.venue} onChange={e => ChangeField(e, "venue")} type="text" placeholder="Место проведения события"/>
            <Input value={eventDetails.maxParticipants} onChange={e => ChangeField(e, "maxParticipants")} type="number" placeholder="Максимальное количество участников"/>
            <Input value={eventDetails.dateTime} onChange={e => ChangeField(e, "dateTime")} type="datetime-local" placeholder="Дата и время проведения"/>
            <Input type="file" onChange={handleFileChange} className={classes.inputFile}/>
            <div className={classes.buttonContainer}>
                <CancelButton onClick={handleCancel}>Отмена</CancelButton>
                <SubmitButton type="submit">{isCreating ? "Создать" : "Подтвердить"}</SubmitButton>
            </div>
        </form>
    );
}

export default CreateUpdateEventForm;