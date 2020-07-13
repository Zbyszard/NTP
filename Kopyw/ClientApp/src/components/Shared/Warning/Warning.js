import React from 'react';
import PropTypes from 'prop-types';
import classes from './Warning.module.css';
import Communicate from '../Communicate/Communicate';
import Backdrop from '../Backdrop/Backdrop';
import Button from '../Button/Button';

const Warning = props => {

    return (
        <>
            <Communicate zIndex={2} position={"fixed"}>
                <div className={classes.message}>
                    {props.message}
                </div>
                <div className={classes.buttonContainer}>
                    <Button onClick={props.cancelCallback}>Cancel</Button>
                    <Button onClick={props.confirmCallback}>Confirm</Button>
                </div>
            </Communicate>
            <Backdrop clickHandler={props.cancelCallback}
                zIndex={1}
                isActive={true} />
        </>
    );
}

Warning.propTypes = {
    message: PropTypes.any.isRequired,
    cancelCallback: PropTypes.func,
    confirmCallback: PropTypes.func
}

export default Warning;