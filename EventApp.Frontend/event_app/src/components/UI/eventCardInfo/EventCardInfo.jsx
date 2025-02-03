import React from "react";
import classes from "./EventCardInfo.module.css"

const EventCardInfo = ({_event}) => {

    const {id, title, description, location, venue, eventDateTime, participants, maxParticipants, imageBase64} = _event;

    const date = new Date(eventDateTime);

    return (
        <div className={classes.eventDetailsContainer}>
            <h2 className={classes.eventTitle}>{title}</h2>
            <img className={classes.eventImage} src={imageBase64} alt={title} />
            <p className={classes.eventDescription}>{description}</p>
            <p className={classes.eventLocation}>Место проведения: {venue}</p>
            <p className={classes.eventDateTime}>Дата: {date.toLocaleDateString("ru-RU")}, Время: {date.toLocaleTimeString("ru-RU")}</p>

            <h3 className={classes.participantsTitle}>Список участников:</h3>
            <div className={classes.participantsList}>
                {participants && participants.length > 0 ? (
                    participants.map((participant) => (
                        <div key={id} className={classes.participantItem}>
                            <p>{participant.participant.name} {participant.participant.surname}</p>
                            <p>Дата и время регистрации: {new Date(participant.registrationDate).toLocaleString()}</p>
                        </div>
                    ))
                ) : (
                    <p>Нет участников на данный момент.</p>
                )}
            </div>
        </div>
    );
}

export default EventCardInfo;