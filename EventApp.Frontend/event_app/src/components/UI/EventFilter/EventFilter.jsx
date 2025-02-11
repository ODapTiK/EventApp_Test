import React, {useState} from "react";
import Input from "../input/Input"
import SubmitButton from "../buttons/submitButton/SubmitButton";
import classes from "./EventFilter.module.css"
import CancelButton from "../buttons/cancelButton/CancelButton";
import baseUrl from "../../Utils/baseUrl";

const EventFilter = ({setCurrentPage, setEvents, setTotalPages}) => {

    const [title, setTitle] = useState('');
    const [date, setDate] = useState('');
    const [category, setCategory] = useState('');
    const [location, setLocation] = useState('');

    const handleSearch = (e) => {
        e.preventDefault();
        const body = {
            Title: title,
            Date: date || new Date(0),
            Category: category,
            Location: location,
            Page: 1
        }
        fetch(`${baseUrl}api/Events/Filter`, {
            method: "PUT",
            body: JSON.stringify(body),
            headers: {"Content-type": "application/json"}
        })
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
    };

    const handleReset = (e) => {
        e.preventDefault();
        setTitle('');
        setDate('');
        setCategory('');
        setLocation('');
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
    }

    return(
        <div className={classes.searchSortContainer}>
            <h2 className={classes.searchTitle}>Поиск</h2>
            <form onSubmit={handleSearch} className={classes.inputFields}>
                <Input 
                    value={title} 
                    onChange={(e) => setTitle(e.target.value)} 
                    placeholder="Поиск по названию" 
                />
                <Input 
                    type="date" 
                    value={date} 
                    onChange={(e) => setDate(e.target.value)} 
                />
                <Input 
                    value={category} 
                    onChange={(e) => setCategory(e.target.value)} 
                    placeholder="Введите категорию" 
                />
                <Input 
                    value={location} 
                    onChange={(e) => setLocation(e.target.value)} 
                    placeholder="Введите место проведения" 
                />
                <CancelButton onClick={handleReset}>Сброс</CancelButton>
                <SubmitButton type="submit">Поиск</SubmitButton>
            </form>
        </div>
    );
}

export default EventFilter;