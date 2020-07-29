import React from 'react';
import PropTypes from 'prop-types';
import css from './PageContainer.module.css';

const PageContainer = props => {
    
    const classList = [css.pageContainer];
    if (props.mobileHidden)
        classList.push(css.toLeft);
    const classes = classList.join(' ');

    return (
        <div className={classes}>
            {props.children}
        </div>
    );
}

PageContainer.propTypes = {
    mobileHidden: PropTypes.bool
}

export default PageContainer;