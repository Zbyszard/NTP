import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import PageLink from './PageLink';
import PropTypes from 'prop-types';
import css from './PageSelector.module.css';

const PageSelector = props => {
    const slideHandler = e => {
        const value = e.target.value;
        setSliderValue(+value);
    }
    const [lastClickTime, setLastClickTime] = useState(null);
    const [sliderValue, setSliderValue] = useState(props.value);

    useEffect(() => {
        props.setMax(props.pagesCount > 10 ? props.pagesCount - 9 : 1);
    }, [props.pagesCount]);

    const visiblePagesCount = props.pagesCount > 10 ? 10 : props.pagesCount;
    const firstPageNumber = sliderValue > props.pagesCount - 9 ?
        props.pagesCount - 9 <= 1 ? 1 : props.pagesCount - 9 :
        sliderValue;
    const visiblePages = Array.from(Array(visiblePagesCount),
        (el, index) => {
            let key = +firstPageNumber + index;
            return <PageLink number={key}
                key={key}
                url={props.url}
                active={props.currentPage === key} />
        });
    let slider = null;
    if (props.pagesCount > 10) {
        slider = <input className={css.slider}
            onChange={slideHandler}
            type="range"
            value={sliderValue}
            step={1} min={1} max={props.max} />;
    }
    return (
        <div className={css.pageSelector}>
            <div className={css.linkContainer}>
                {visiblePages}
            </div>
            {slider}
            <div className={css.navButtons}>
                <Link className={css.prev}
                    to={props.currentPage < 3 ? props.url : `${props.url}/${props.currentPage - 1}`} />
                <Link className={css.next}
                    to={props.currentPage > props.pagesCount - 1 ?
                        `${props.url}/${props.pagesCount}` :
                        `${props.url}/${props.currentPage + 1}`} />
            </div>
        </div>
    );
}

PageSelector.propTypes = {
    pagesCount: PropTypes.number.isRequired,
    currentPage: PropTypes.number,
    url: PropTypes.string.isRequired,
    max: PropTypes.number.isRequired,
    value: PropTypes.number.isRequired,
    setMax: PropTypes.func
}

export default PageSelector;