import React, { Component } from 'react';
import PropTypes from 'prop-types';
import classes from '../Menu.module.css';
import '../../../Shared/Icons/css/fontello.css';

class Search extends Component {
    constructor(props) {
        super(props);
        this.inputRef = React.createRef();
        this.state = {
            showSearch: false,
            searchString: "",
            iconSize: "1rem"
        };
    }

    componentDidMount() {
        let inputHeight = `${this.inputRef.current.clientHeight * 0.8}px`;
        this.setState({ iconSize: inputHeight });
    }

    render() {
        let barClasses = [classes.searchBar];
        let iconClasses = [classes.searchIcon];
        if (this.state.showSearch)
            barClasses.push(classes.active);
        else
            iconClasses.push(classes.active);
        let barClassList = barClasses.join(' ');
        let iconClassList = iconClasses.join(' ');
        return (
            <>
                <input className={barClassList}
                    value={this.state.searchString}
                    onChange={this.inputChangeHandler}
                    type="text"
                    placeholder="What are you looking for?"
                    ref={this.inputRef} 
                    onBlur={this.disableSearch}/>
                <div className={iconClassList}
                    onClick={this.enableSearch}>
                    <i style={{ fontSize: this.state.iconSize }} className="icon-search" />
                </div>
            </>
        );
    }

    inputChangeHandler = e => {
        this.setState({searchString: e.target.value});
    }

    enableSearch = () => {
        this.setState({ showSearch: true });
        this.inputRef.current.focus();
    }
    disableSearch = () => {
        if(this.state.searchString === "")
            this.setState({ showSearch: false });
    }

}

// SearchBar.propTypes = {

// }

export default Search;