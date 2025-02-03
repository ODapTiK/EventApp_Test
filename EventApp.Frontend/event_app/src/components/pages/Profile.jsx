import React, { useState, useEffect } from "react";
import cl from "../../styles/ContentContainer.module.css"
import EventCardsList from "../UI/EventCardsList/EventCardsList";
import Pagination from "../UI/Pagination/Pagination";
import { useAccessToken } from "../Utils/AuthContext";

const Profile = ({user, setEventModalVisibility, setCurrentEvent, setIsCreatingUser=()=>{}, setIsLoginModal=()=>{}, 
    setLoginModalVisibility=()=>{}, setCurrentUser}) => {

    const accessToken = useAccessToken();
    const [userEvents, setUserEvents] = useState([]);
    const [totalUserPages, setTotalUserPages] = useState(1);
    const [currentUserPage, setCurrentUserPage] = useState(1);

    const birthDateLocale = new Date(user.birthDate).toLocaleDateString("ru-RU");

    const handleUpdateUserInfo = () => {
        setIsCreatingUser(false);
        setIsLoginModal(false);
        setLoginModalVisibility(true);
    }

    const updateEvents = (page) => {
        fetch(`https://localhost:7164/api/Participant/Events/${page}`,
            {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${accessToken}`
                }
            }
        )
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status);
                }
                return response.json();
            })
            .then((jsonObject) => {
                setUserEvents(jsonObject.items);
                setTotalUserPages(jsonObject.totalPages);
                setCurrentUserPage(page);
            })
            .catch((e) => {
                console.error(e.message);
            })
    }

    useEffect(() => {
        fetch("https://localhost:7164/api/Participant/Events/1",
            {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${accessToken}`
                }
            }
        )
            .then((response) => {
                if(!response.ok){
                    throw new Error(response.status);
                }
                return response.json();
            })
            .then((jsonObject) => {
                setUserEvents(jsonObject.items);
                setTotalUserPages(jsonObject.totalPages);
                setCurrentUserPage(1);
            })
            .catch((e) => {
                console.error(e.message);
            })
      }, [])   

    return(
        <div className={cl.container}>
            <div>
                <h2 className={cl.eventListTitle}>
                    События, на которые вы подписаны
                </h2>
                <div className={cl.eventList}>
                    <EventCardsList isAdmin={false} 
                            currentUser={user}
                            events={userEvents} 
                            setModalVisibility={setEventModalVisibility}
                            setCurrentEvent={setCurrentEvent}
                            setCurrentUser={setCurrentUser}/>
                </div>
                <Pagination totalPages={totalUserPages} currentPage={currentUserPage} onPageChange={updateEvents}/>
            </div>
            <div className={cl.filterContainer}>
                <div>
                    <div className={cl.infoTitle}>
                        <h3>
                            Информация об участнике
                        </h3>
                    </div>
                    <div className={cl.userInfo}>
                        <div className={cl.userInfoItem}>
                            <span className={cl.label}>Имя:</span>
                            <span className={cl.value}>{user.name}</span>
                        </div>
                        <div className={cl.userInfoItem}>
                            <span className={cl.label}>Фамилия:</span>
                            <span className={cl.value}>{user.surname}</span>
                        </div>
                        <div className={cl.userInfoItem}>
                            <span className={cl.label}>Почта:</span>
                            <span className={cl.value}>{user.email}</span>
                        </div>
                        <div className={cl.userInfoItem}>
                            <span className={cl.label}>Дата рождения:</span>
                            <span className={cl.value}>{birthDateLocale}</span>
                        </div>
                    </div>
                    <div>
                        <button className={cl.changeInfoButton} onClick={handleUpdateUserInfo}>
                            Изменить данные
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Profile;