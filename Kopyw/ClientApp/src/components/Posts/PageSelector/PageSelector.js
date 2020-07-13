import React, { useEffect, useState } from 'react';
import PageLink from './PageLink';
import PropTypes from 'prop-types';
import classes from './PageSelector.module.css';

const PageSelector = props => {
    const slideHandler = e => {
        const value = e.target.value;
        props.setValue(+value);
    }
    const [lastClickTime, setLastClickTime] = useState(null);
    useEffect(() => {
        props.setValue(props.currentPage);
    }, []);

    useEffect(() => {
        props.setMax(props.pagesCount > 10 ? props.pagesCount - 9 : 1);
    }, [props.pagesCount]);

    const onLinkClick = pageNumber => {
        let now = new Date();
        if(lastClickTime !== null && now - lastClickTime < 500)
            return;
        setLastClickTime(now);
        props.linkClickCallback(pageNumber);
    }

    const visiblePagesCount = props.pagesCount > 10 ? 10 : props.pagesCount;
    const firstPageNumber = props.value > props.pagesCount - 9 ?
        props.pagesCount - 9 <= 1 ? 1 : props.pagesCount - 9 :
        props.value;
    const visiblePages = Array.from(Array(visiblePagesCount),
        (el, index) => {
            let key = +firstPageNumber + index;
            return <PageLink number={key}
                key={key}
                url={props.url}
                clickCallback={onLinkClick}
                active={props.currentPage === key} />
        });
    let slider = null;
    if (props.pagesCount > 10) {
        slider = <input className={classes.slider}
            onChange={slideHandler}
            type="range"
            value={props.value}
            step={1} min={1} max={props.max} />;
    }
    return (
        <div className={classes.pageSelector}>
            <div className={classes.linkContainer}>
                {visiblePages}
            </div>
            {slider}
        </div>
    );
}

PageSelector.propTypes = {
    pagesCount: PropTypes.number.isRequired,
    currentPage: PropTypes.number,
    url: PropTypes.string.isRequired,
    max: PropTypes.number.isRequired,
    value: PropTypes.number.isRequired,
    setMax: PropTypes.func,
    setValue: PropTypes.func,
    linkClickCallback: PropTypes.func
}

export default PageSelector;