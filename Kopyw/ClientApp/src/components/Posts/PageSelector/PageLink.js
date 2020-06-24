import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import classes from './PageSelector.module.css';

const PageLink = props => {
    const url = `${props.url}${props.number == 1 ? '' : props.number}`;
    const clickHandler = () => {
        props.clickCallback(props.number);
    }
    let linkClasses = [classes.pageLink];
    if(props.active)
        linkClasses.push(classes.active);
    const linkClassList = linkClasses.join(' ');
    return (
        <Link to={url} onClick={clickHandler}
            className={linkClassList}>
            {props.number}
        </Link>
    );
}

PageLink.propTypes = {
    url: PropTypes.string.isRequired,
    number: PropTypes.number.isRequired
}

export default PageLink;