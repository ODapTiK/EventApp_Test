import React from 'react';
import cl from './Pagination.module.css'

const Pagination = ({ totalPages, currentPage, onPageChange }) => {
    const handleClick = (pageNumber) => {
        if (pageNumber !== currentPage) {
            onPageChange(pageNumber); 
        }
    };

    return (
        <div className={cl.paginationContainer}>
            {Array.from({ length: totalPages }, (_, index) => (
                <button
                    key={index}
                    className={`
                        ${cl.paginationButton} 
                        ${currentPage === index + 1 ? cl.active : ''}`}
                    onClick={() => handleClick(index + 1)}
                >
                    {index + 1}
                </button>
            ))}
        </div>
    );
};

export default Pagination;