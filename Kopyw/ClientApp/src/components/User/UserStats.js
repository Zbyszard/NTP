import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { UserApiConstants } from '../../Shared/ApiConstants/ApiConstants';
import Communicate from '../Shared/Communicate/Communicate';
import LoadingSymbol from '../Shared/LoadingSymbol/LoadingSymbol';
import axios from 'axios';
import css from './UserStats.module.css';

class UserStats extends Component {

    state = {
        isFollowed: false,
        postCount: 0,
        commentCount: 0,
        pointsFromPosts: 0,
        pointsFromComments: 0,
        isLoading: true,
        dataAvailable: false,
        errorMessage: null
    }

    componentDidMount = () => {
        this.requestData();
    }

    componentDidUpdate = (prevProps, prevState) => {
        if (prevProps.userName !== this.props.userName)
            this.requestData();
    }

    requestData = () => {
        this.setState({ isLoading: true, errorMessage: null });
        let url = `${UserApiConstants.getStats}/${this.props.userName}`;
        axios.get(url)
            .then(r => {
                if (r.status === 200) {
                    let { userName, ...stats } = { ...r.data };
                    this.setState({ ...stats, isLoading: false, dataAvailable: true });
                }
            })
            .catch(e => {
                if (e.response.status === 404) {
                    this.setState({ errorMessage: "User not found" });
                }
                this.setState({ isLoading: false, dataAvailable: false });
            })
    }

    render() {
        let body;
        if (!this.state.isLoading && !this.state.dataAvailable) {
            if (this.state.errorMessage)
                body = <div>{this.state.errorMessage}</div>
            else
                body = <div>An error occured</div>;
        }
        else if (!this.state.isLoading) {
            body =
                <>
                    <p className={css.stat}>Posts: {this.state.postCount}</p>
                    <p className={css.stat}>Comments: {this.state.commentCount}</p>
                    <p className={css.stat}>Post score: {this.state.pointsFromPosts}</p>
                    <p className={css.stat}>Comment score: {this.state.pointsFromComments}</p>
                </>;
        }
        else
            body = <div className={css.center}><LoadingSymbol /></div>;
        return (
            <div className={css.container}>
                <h1 className={css.name}>{this.props.userName}</h1>
                {body}
            </div>
        );
    }
}

UserStats.propTypes = {
    userName: PropTypes.string.isRequired
}

export default UserStats;