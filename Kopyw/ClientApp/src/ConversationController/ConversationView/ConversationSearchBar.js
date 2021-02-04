import React, { useState, useRef, useContext, useEffect } from 'react';
import PropTypes from 'prop-types';
import MessagingContext from '../../Context/MessagingContext';
import css from './ConversationView.module.css';
import AuthContext from '../../Context/AuthContext';
import SearchResult from '../../components/Conversation/SearchResult';
import getConversationDisplayName from '../../Shared/Functions/getConversationDisplayName';

const ConversationSearchBar = props => {
    const [searchString, setSearchString] = useState("");
    const [showResults, setShowResults] = useState(false);
    const authContext = useContext(AuthContext);
    const messagingContext = useContext(MessagingContext);
    const inputRef = useRef();
    const hideResultsTimeout = useRef(0);

    const inputChangeHandler = e => {
        let value = e.target.value;
        setSearchString(value);
        setShowResults(value !== "");

        messagingContext.searchConversations(value);
    }

    const inputFocusHandler = () => {
        if (hideResultsTimeout.current !== 0) {
            clearTimeout(hideResultsTimeout.current);
            hideResultsTimeout.current = 0;
        }
        setShowResults(searchString !== "");
    }

    const inputBlurHandler = () => {
        hideResultsTimeout.current = setTimeout(() => {
            setShowResults(false);
        }, 400);
    }

    const createConversation = conversationObject => {
        messagingContext.createConversation(conversationObject);
    }

    const enterConversation = id => {
        messagingContext.enterConversation(id);
    }

    useEffect(() => {
        return () => {
            clearTimeout(hideResultsTimeout.current);
            hideResultsTimeout.current = 0;
        }
    }, [])

    let searchResults = null;
    if (showResults && messagingContext.searchResults.length > 0)
        searchResults =
            <div className={css.resultContainer}>
                {messagingContext.searchResults.map(r => {
                    let callback = r.id !== 0 ?
                        () => enterConversation(r.id) :
                        () => createConversation(r);
                    let name = getConversationDisplayName(r, authContext.userName);
                    return <SearchResult class={css.result} key={r.userNames}
                        clickCallback={callback}
                        displayName={name} />;
                })}
            </div>;

    return (
        <div className={css.searchContainer}>
            <input type="search" ref={inputRef}
                value={searchString}
                onChange={inputChangeHandler}
                placeholder="Find users or groups"
                onBlur={inputBlurHandler}
                onFocus={inputFocusHandler} />
            {searchResults}
        </div>
    );
}

ConversationSearchBar.propTypes = {
    changeCallback: PropTypes.func
}

export default ConversationSearchBar;