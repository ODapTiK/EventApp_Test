import React from "react";
import EventCard from "../eventCard/EventCard";
import classes from "./EventCardsList.module.css";

const EventCardsList = ({events, isAdmin, setModalVisibility, setUpdateEventModalVisibility, 
                setCurrentEvent, setIsCreatingEvent, setUpdateId, currentUser, setEvents, setCurrentUser}) => {
    
    return (
        <div className={classes.eventContainer}>
            {events.map((_event) => 
                <EventCard _event={_event}
                            isAdmin={isAdmin} 
                            setUpdateEventModalVisibility={setUpdateEventModalVisibility} 
                            key={_event.id} 
                            setCurrentEvent={setCurrentEvent}
                            setModalVisibility={setModalVisibility}
                            setIsCreatingEvent={setIsCreatingEvent}
                            setUpdateId={setUpdateId}
                            currentUser={currentUser}
                            setEvents={setEvents}
                            isParticipating={currentUser.events.some(event => event.eventId == _event.id)}
                            setCurrentUser={setCurrentUser}/>
            )}
        </div>  
    );
}

export default EventCardsList;