import React from 'react';
import PageLink from './PageLink';
import PropTypes from 'prop-types';
import classes from './PageSelector.module.css';

const PageSelector = props => {
    const pages = Array.from(Array(props.pagesCount),
        (el, index) => <PageLink number={index + 1}
            key={index + 1}
            url={props.url}
            clickCallback={props.onLinkClick}
            active={props.currentPage == index + 1} />);
    return (
        <div className={classes.pageSelecor}>
            {pages}
        </div>
    );
}

PageSelector.propTypes = {
    pagesCount: PropTypes.number.isRequired,
    currentPage: PropTypes.number,
    url: PropTypes.string.isRequired,
    onLinkClick: PropTypes.func
}

export default PageSelector;