import React, { Component } from 'react';
import PropTypes from 'prop-types';
import classes from './CommentForm.module.css';

class CommentForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            text: "",
            validationMessages: []
        };
    }

    render() {
        let messages = !this.state.validationMessages.length ? null :
            this.state.validationMessages.map(msg => <li key={msg}>{msg}</li>);
        return (
            <div className={classes.container}>
                <form onSubmit={this.submitHandler}>
                    <textarea className={classes.text}
                        value={this.state.text}
                        onChange={this.changeHandler}
                        name="text"
                        placeholder="Your comment" />
                    {!messages ? null :
                        <ul className={classes.validationMessages}>{messages}</ul>}
                    <input type="submit" value="Send" />
                </form>
            </div>
        );
    }

    changeHandler = e => {
        const value = e.target.value;
        this.setState({
            text: value,
            validationMessages: []
        });
    }

    submitHandler = e => {
        e.preventDefault();
        if (!this.validate())
            return;
        this.sendComment();
    }

    sendComment = () => {
        throw new Error("send comment not implemented");
    }

    validate = () => {
        let isOk = true;
        let messages = [];
        let regex = /\s/g;
        let text = this.state.text;
        text = text.replace(regex, '');
        if (text.length < 5) {
            messages.push("Your comment needs to be at least 5 characters long");
            isOk = false;
        }
        this.setState({ validationMessages: messages });
        return isOk;
    }
}

CommentForm.propTypes = {
    comments: PropTypes.arrayOf(PropTypes.object)
}

export default CommentForm;