import React, {useEffect, useState} from "react";
import EventActionButton from "../buttons/EventActionButton/EventActionButton";
import classes from "./EventCard.module.css"
import { useAccessToken } from "../../Utils/AuthContext";
import baseUrl from "../../Utils/baseUrl";

const EventCard = ({_event, isAuthenticated, isAdmin=false, setUpdateEventModalVisibility=()=>{}, setCurrentEvent=()=>{}, 
                    setModalVisibility, setIsCreatingEvent, setUpdateId, setEvents, isParticipating,
                    setCurrentUser}) => {

    const [participation, setIsParticipation] = useState(isParticipating);

    useEffect(() => {
        setIsParticipation(isParticipating);
    }, [isParticipating])

    const {id, title, description, venue, category, eventDateTime, participants, maxParticipants, image} = _event;

    const accessToken = useAccessToken();

    const date = new Date(eventDateTime);

    const onCardClick = () => {
        setCurrentEvent(_event);
        setModalVisibility(true);
    }

    const handleParticipation = (e) =>{
        e.stopPropagation();
        if(!participation){
            fetch(`${baseUrl}api/Participant/Events/Subscribe/${_event.id}`, {
                method: "GET",
                headers: {"Authorization": `Bearer ${accessToken}`}
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status)
                }
                else{
                    setIsParticipation(!participation);
                    const {id, ...rest} = _event
                    setCurrentUser((user) => ({...user, events: [...user.events, {eventId: id, ...rest}]}))
                }
            })
            .catch((e) => {
                console.error(e.message);
            })
        }
        else{
            fetch(`${baseUrl}api/Participant/Events/Unsubscribe/${_event.id}`, {
                method: "GET",
                headers: {"Authorization": `Bearer ${accessToken}`}
            })
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status)
                }
                else{
                    setIsParticipation(!participation);
                    setCurrentUser((user) => ({...user, events: user.events.filter(e => e.eventId !== _event.id)}))
                }
            })
            .catch((e) => {
                console.error(e.message);
            })
        }
    }

    const handleUpdate = (e) => {
        e.stopPropagation();
        setUpdateId(id);
        setIsCreatingEvent(false);
        setUpdateEventModalVisibility(true);
    }

    const handleDelete = (e) => {
        e.stopPropagation();
        fetch(`${baseUrl}api/Events/${_event.id}`, {
            method: "DELETE",
            headers: {"Authorization": `Bearer ${accessToken}`}
        })
        .then((response) => {
            if(!response.ok){
                throw new Error(response.status);
            }
        })
        .then(() => {
            setEvents((events) => events.filter((e) => e.id !== _event.id));
        })
        .catch((e) => {
            console.error(e.message);
        })
    }

    return(
        <div className={classes.container} onClick={onCardClick}>
            <div className={classes.imageContainer}>
                {image ? (
                    <img src={image} alt={title} />
                ) : (
                <div>Нет изображения</div>
                )}
            </div>
            <div className={classes.textContainer}>
                <h3>{title}</h3>
                <p>
                    Категория: {category}
                </p>
                <p>
                    Дата проведения: {date.toLocaleDateString("ru-RU")} 
                </p>
                <p>
                    Время проведения: {date.toLocaleTimeString("ru-RU")}
                </p>
                {participants.length < maxParticipants ? (
                    <p>
                        Участники: {participants.length} из {maxParticipants}
                    </p>
                ) : (
                    <p>
                        Свободных мест нет! Участники: {participants.length} из {maxParticipants}
                    </p>
                )}
                
                {!isAdmin ? (
                    <div className={classes.subscribeButtonContainer}>
                        <div className={classes.participationStatus}>
                            <p>
                                {participation ? "Вы участвуете" : "Вы не участвуете"}
                          </p>
                        </div>
                        <div className={classes.subscribeButton}>
                            <EventActionButton onClick={handleParticipation}>
                                {participation ? "Отписаться" : "Участвовать!"}
                            </EventActionButton>
                        </div>
                    </div>
                ) : (
                    <div className={classes.subscribeButtonContainer}>
                        <EventActionButton onClick={handleUpdate}>
                            Редактировать
                        </EventActionButton>
                        <EventActionButton onClick={handleDelete}>
                            Удалить
                        </EventActionButton>
                    </div>
                )}
                
            </div>
        </div>
    );
}

export default EventCard;