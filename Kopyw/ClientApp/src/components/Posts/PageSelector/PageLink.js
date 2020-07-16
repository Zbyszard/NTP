import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import classes from './PageSelector.module.css';

const PageLink = props => {
    const clickHandler = () => {
        props.clickCallback(props.number);
        window.scroll(0, 0);
    }

    let url = `${props.url}${props.number === 1 ? '' : props.number}`;
    let lastChar = url.charAt(url.length - 1);

    if (!Number.isInteger(+lastChar))
        url = url.substring(0, url.length - 1);

    let linkClasses = [classes.pageLink];
    if (props.active)
        linkClasses.push(classes.active);
    const linkClassList = linkClasses.join(' ');
    return (
        <Link to={url}
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