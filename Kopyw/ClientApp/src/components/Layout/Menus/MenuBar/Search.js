import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import classes from '../Menu.module.css';
import '../../../Shared/Icons/css/fontello.css';

class Search extends Component {
    constructor(props) {
        super(props);
        this.inputRef = React.createRef();
        this.state = {
            showSearch: false,
            searchString: "",
            iconSize: "1rem",
            redirect: null
        };
    }

    componentDidMount() {
        let inputHeight = `${this.inputRef.current.clientHeight * 0.8}px`;
        this.setState({ iconSize: inputHeight });
    }

    inputChangeHandler = e => {
        this.setState({ searchString: e.target.value });
    }

    enableSearch = () => {
        this.setState({ showSearch: true });
        this.inputRef.current.focus();
    }
    disableSearch = () => {
        if (this.state.searchString === "")
            this.setState({ showSearch: false });
    }

    search = e => {
        e.preventDefault();
        let str = this.state.searchString;
        if (!str.replace(/\s/g, '').length) {
            this.setState({ redirect: '/' });
            return;
        }
        this.setState({ redirect: `/search/${str}` });
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
        if (!!this.state.redirect) {
            let redirect = this.state.redirect;
            this.setState({ redirect: null });
            return <Redirect to={redirect} />
        }
        return (
            <>
                <form onSubmit={this.search} id="searchform" />
                <input className={barClassList}
                    form="searchform"
                    value={this.state.searchString}
                    onChange={this.inputChangeHandler}
                    type="text"
                    placeholder="Looking for..."
                    ref={this.inputRef}
                    onBlur={this.disableSearch} />
                <div className={iconClassList}
                    onClick={this.enableSearch}>
                    <i style={{ fontSize: this.state.iconSize }} className="icon-search" />
                </div>
            </>
        );
    }


}

export default Search;