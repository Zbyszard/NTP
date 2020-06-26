import React from 'react';
import classes from './Communicate.module.css';

const Communicate = props => {

    return(
        <div className={classes.communicate}>
            {props.children}
        </div>
    );
}

export default Communicate;