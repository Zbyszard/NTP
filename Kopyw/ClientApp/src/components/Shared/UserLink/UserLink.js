import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import css from './UserLink.module.css';

const UserLink = props => {
    let link =
        <Link className={css.user} to={`/user/${props.user}`}>
            {props.user}
        </Link>;
    if (!props.user)
        link =
            <span className={css.user}>
                [deleted]
            </span>;

    return link;
}

UserLink.propTypes = {
    user: PropTypes.string
}

export default UserLink;