import React, {useState} from "react";
import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";
import Events from "./components/pages/Events";
import Profile from "./components/pages/Profile"
import Header from "./components/UI/Header/Header"
import Modal from "./components/UI/modal/eventCardModal/Modal";
import EventCardInfo from "./components/UI/eventCardInfo/EventCardInfo";
import LoginModal from "./components/UI/modal/loginRegisterModal/LoginModal";
import LoginForm from "./components/UI/forms/LoginForm/LoginForm";
import RegisterParticipantForm from "./components/UI/forms/registerParticipantForm/RegisterParticipantForm";
import AuthContext from "./components/Utils/AuthContext";

function App() {

  const defaultUser = {id: '', name: "Гость", surname: '', email: '', birthDate: '', events: []};

  const [currentUser, setCurrentUser] = useState(defaultUser);
  const [currentEvent, setCurrentEvent] = useState('');
  const [isAdmin, setIsAdmin] = useState(false);
  const [isAuthentificated, setIsAuthentificated] = useState(false);

  const [loginModalVisibility, setLoginModalVisibility] = useState(false);
  const [createUpdateEventModalVisibility, setCreateUpdateEventModalVisibility] = useState(false)
  const [isLoginModal, setIsLoginModal] = useState(true);
  const [eventModalVisibility, setEventModalVisibility] = useState(false);
  const [isCreateingUser, setIsCreatingUser] = useState(true);
  const [isCreatingEvent, setIsCreatingEvent] = useState(true);

  return(
    <AuthContext setIsAdmin={setIsAdmin} setIsAuthenticated={setIsAuthentificated} setCurrentUser={setCurrentUser}>
      <BrowserRouter>
            <Header isAuthenticated={isAuthentificated} isAdmin={isAdmin} user={currentUser} 
                    setIsAdmin={setIsAdmin} setIsAuthentificated={setIsAuthentificated} 
                    setIsLoginModal={setIsLoginModal} setLoginModalVisibility={setLoginModalVisibility} 
                    setCreateEventModalVisibility={setCreateUpdateEventModalVisibility}
                    setIsCreatingUser={setIsCreatingUser} setCurrentUser={setCurrentUser}
                    setIsCreatingEvent={setIsCreatingEvent}/>
        <Routes>
          <Route path="/Events" element={<Events isAdmin={isAdmin}  
                                createUpdateEventModalVisibility={createUpdateEventModalVisibility}
                                setCreateUpdateEventModalVisibility={setCreateUpdateEventModalVisibility}
                                setEventModalVisibility={setEventModalVisibility}
                                setCurrentEvent={setCurrentEvent}
                                isCreatingEvent={isCreatingEvent}
                                setIsCreatingEvent={setIsCreatingEvent}
                                currentUser={currentUser}
                                setCurrentUser={setCurrentUser}/>}/>
          <Route path="/Profile" element={isAuthentificated ? <Profile user={currentUser}
                                                  setCurrentEvent={setCurrentEvent}
                                                  setEventModalVisibility={setEventModalVisibility}
                                                  setIsCreatingUser={setIsCreatingUser}
                                                  setIsLoginModal={setIsLoginModal}
                                                  setLoginModalVisibility={setLoginModalVisibility}
                                                  setCurrentUser={setCurrentUser}/> : <Navigate to="/Events"/>}/>
          <Route path="*" element={<Navigate to="/Events"/>}/>
        </Routes>
        <Modal modalVisibility={eventModalVisibility} setModalVisibility={setEventModalVisibility}>
          <EventCardInfo _event={currentEvent}/>
        </Modal>
        <LoginModal modalVisibility={loginModalVisibility} login={isLoginModal} setModalVisibility={setLoginModalVisibility} setIsLoginModal={setIsLoginModal}>
          <RegisterParticipantForm isCreating={isCreateingUser} setModalVisibility={setLoginModalVisibility} setCurrentUser={setCurrentUser}/>
          <LoginForm setModalVisibility={setLoginModalVisibility} setCurrentUser={setCurrentUser} 
                      setIsAdmin={setIsAdmin} setIsAuthenticated={setIsAuthentificated}/>
        </LoginModal>
      </BrowserRouter>
    </AuthContext>
  );
}

export default App;
