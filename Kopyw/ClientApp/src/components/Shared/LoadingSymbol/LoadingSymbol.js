import React from 'react';
import classes from './LoadingSymbol.module.css';

const LoadingSymbol = () => {
    return(
        <div className={classes.container}>
            <div className={classes.dot}/>
        </div>
    );
}

export default LoadingSymbol;