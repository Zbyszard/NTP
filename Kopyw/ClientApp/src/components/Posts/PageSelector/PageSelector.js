import React, { useState, useEffect } from 'react';
import PageLink from './PageLink';
import PropTypes from 'prop-types';
import classes from './PageSelector.module.css';

const PageSelector = props => {
    const [sliderValue, setSliderValue] = useState(1);
    const [sliderMax, setSliderMax] = useState(1);
    const slideHandler = e => {
        const value = e.target.value;
        setSliderValue(+value);
    }

    useEffect(() => {
        setSliderValue(props.currentPage);
    }, []);

    useEffect(() => {
        setSliderMax(props.pagesCount > 10 ? props.pagesCount - 9 : 1);
    }, [props.pagesCount]);

    let showSlider = props.pagesCount > 10;
    const visiblePagesCount = props.pagesCount > 10 ? 10 : props.pagesCount;
    const firstPageNumber = sliderValue > props.pagesCount - 9 ?
        props.pagesCount - 9 < 1 ? 1 : props.pagesCount - 9 :
        sliderValue;
    const visiblePages = Array.from(Array(visiblePagesCount),
        (el, index) => {
            let key = +firstPageNumber + index;
            return <PageLink number={key}
                key={key}
                url={props.url}
                clickCallback={props.onLinkClick}
                active={props.currentPage === key} />
        });
    let slider = null;
    if (showSlider) {
        slider = <input className={classes.slider}
            onChange={slideHandler}
            type="range"
            value={sliderValue}
            step={1} min={1} max={sliderMax} />;
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
    onLinkClick: PropTypes.func
}

export default PageSelector;