import React, {useState, useEffect} from "react";
import "../../global.css";
import cl from "../../styles/ContentContainer.module.css"
import Modal from "../UI/modal/eventCardModal/Modal";
import CreateUpdateEventForm from "../UI/forms/CreateUpdateEventForm/CreateUpdateEventForm";
import EventCardsList from "../UI/EventCardsList/EventCardsList";
import EventFilter from "../UI/EventFilter/EventFilter";
import Pagination from "../UI/Pagination/Pagination";
import baseUrl from "../Utils/baseUrl";

function Events({isAdmin,
                createUpdateEventModalVisibility,
                setCreateUpdateEventModalVisibility,
                setEventModalVisibility,
                setCurrentEvent,
                isCreatingEvent,
                setIsCreatingEvent,
                currentUser,
                setCurrentUser}) {

  const [events, setEvents] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);
  const [updateId, setUpdateId] = useState('');

  const updateEvents = (page) => {
    fetch(`${baseUrl}api/Events/${page}`)
        .then((response) => {
            if(!response.ok){
                throw new Error(response.status);
            }
            return response.json();
        })
        .then((jsonObject) => {
            setEvents(jsonObject.items);
            setTotalPages(jsonObject.totalPages);
            setCurrentPage(page);
        })
        .catch((e) => {
            console.error(e.message);
        })
  } 

  useEffect(() => {
    fetch(`${baseUrl}api/Events/1`)
        .then((response) => {
            if(!response.ok){
                throw new Error(response.status);
            }
            return response.json();
        })
        .then((jsonObject) => {
            setEvents(jsonObject.items);
            setTotalPages(jsonObject.totalPages);
            setCurrentPage(1);
        })
        .catch((e) => {
            console.error(e.message);
        })
  }, [])                  

  return (
    <div className="App">
      <div className={cl.container}>
        <div className={cl.filterContainer}>
          <EventFilter setEvents={setEvents}
                       setTotalPages={setTotalPages}
                       setCurrentPage={setCurrentPage}/>
        </div>
        <div>
          <h2 className={cl.eventListTitle}>
            События, которые скоро произойдут
          </h2>
          <div className={cl.eventList}>
            <EventCardsList isAdmin={isAdmin} 
                            events={events} 
                            setModalVisibility={setEventModalVisibility}
                            setUpdateEventModalVisibility={setCreateUpdateEventModalVisibility}
                            setCurrentEvent={setCurrentEvent}
                            setIsCreatingEvent={setIsCreatingEvent}
                            setUpdateId={setUpdateId}
                            currentUser={currentUser}
                            setEvents={setEvents}
                            setCurrentUser={setCurrentUser}/>
          </div>
          <Pagination onPageChange={updateEvents} totalPages={totalPages} currentPage={currentPage}/>
        </div>
      </div>
      

      <Modal modalVisibility={createUpdateEventModalVisibility} setModalVisibility={setCreateUpdateEventModalVisibility}>
        <CreateUpdateEventForm isCreating={isCreatingEvent} updateId={updateId} 
                        setModalVisibility={setCreateUpdateEventModalVisibility} setEvents={setEvents}/>
      </Modal>
    </div>
  );
}

export default Events;
