import React, { useState, useContext } from 'react';
import { withRouter } from 'react-router-dom';
import MenuContext from '../MenuContext';
import css from '../Menu.module.css';

const Search = props => {
    const menuContext = useContext(MenuContext);
    const [searchString, setSearchString] = useState("");

    const inputChangeHandler = e => {
        setSearchString(e.target.value);
    };

    const searchBlurHandler = () => {
        if (searchString === "")
            menuContext.disableSearch();
    }

    const search = e => {
        e.preventDefault();
        let redirectUrl;
        let str = searchString;
        if (!str.replace(/\s/g, '').length)
            redirectUrl = "/"
        else
            redirectUrl = `/search/${str}`;
        props.history.push(redirectUrl);
    };

    let barClasses = [css.searchBar];
    if (menuContext.showSearch)
        barClasses.push(css.active);
    let barClassList = barClasses.join(' ');
    return (
        <>
            <form onSubmit={search} id="searchform" />
            <input className={barClassList}
                form="searchform"
                value={searchString}
                onChange={inputChangeHandler}
                type="text"
                placeholder="Looking for..."
                ref={menuContext.searchInputRef}
                onBlur={searchBlurHandler} />
        </>
    );
}

export default withRouter(Search);