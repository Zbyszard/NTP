import React from 'react'
import PropTypes from 'prop-types';

const SearchResult = props => {

    return (
        <div className={props.class}
            onClick={props.clickCallback}>
            {props.displayName}
        </div>
    );
}

SearchResult.propTypes = {
    displayName: PropTypes.string.isRequired,
    clickCallback: PropTypes.func,
    class: PropTypes.string
}

export default SearchResult;