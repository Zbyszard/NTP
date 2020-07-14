import React, { Component } from 'react';
import PropTypes from 'prop-types';
import classes from './CommentForm.module.css';
import axios from 'axios';
import Button from '../Shared/Button/Button';

class CommentForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            text: props.text || "",
            validationMessages: []
        };
        this.editMode = !!props.text;
    }

    render() {
        let messages = !this.state.validationMessages.length ? null :
            this.state.validationMessages.map(msg => <li key={msg}>{msg}</li>);
        let cancelButton = null;
        if (this.editMode)
            cancelButton = <Button onClick={this.props.cancelCallback}>Cancel</Button>;
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
                    <div className={classes.buttons}>
                        {cancelButton}
                        <Button onClick={this.submitHandler}>Send</Button>
                    </div>
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
        e && e.preventDefault();
        if(this.props.isBlocked)
            return;
        if (!this.validate())
            return;
        let comment = {
            postId: this.props.postId,
            text: this.state.text
        };
        this.props.submitCallback(comment);
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
    postId: PropTypes.number.isRequired,
    isBlocked: PropTypes.bool,
    text: PropTypes.string,
    submitCallback: PropTypes.func,
    cancelCallback: PropTypes.func
}

export default CommentForm;