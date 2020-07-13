import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Button from '../Shared/Button/Button';
import classes from './PostForm.module.css';

class PostForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            title: props.title || "",
            text: props.text || "",
            validationMessages: []
        }
        this.editMode = props.title && props.text;
        this.formRef = React.createRef();
    }

    render() {
        let messages = !this.state.validationMessages.length ? null :
            this.state.validationMessages.map(msg => <li key={msg}>{msg}</li>);
        let cancelButton = !this.editMode ? null :
            <Button onClick={this.props.cancelCallback}>Cancel</Button>;
        return (
            <div className={classes.container}>
                <form onSubmit={this.submitHandler} ref={this.formRef}>
                    <input className={classes.title}
                        value={this.state.title}
                        onChange={this.changeHandler}
                        name="title"
                        placeholder="Title"
                        onKeyDown={this.preventEnter} />
                    <textarea className={classes.text}
                        value={this.state.text}
                        onChange={this.changeHandler}
                        name="text"
                        placeholder="Write something" />
                    {!messages ? null :
                        <ul className={classes.validationMessages}>{messages}</ul>}
                    <div className={classes.buttons}>
                        {cancelButton}
                        <Button onClick={this.submitHandler}>Submit</Button>
                    </div>
                </form>
            </div>
        );
    }

    preventEnter = e => {
        if (e.key === "Enter")
            e.preventDefault();
    }

    changeHandler = e => {
        const value = e.target.value;
        const name = e.target.name;
        this.setState({
            [name]: value,
            validationMessages: []
        });
    }

    submitHandler = e => {
        e && e.preventDefault();
        if (this.props.isBlocked)
            return;
        if (!this.validateFields())
            return;
        const post = {
            Title: this.state.title,
            Text: this.state.text
        };
        this.props.postCallback(post);
    }

    validateFields = () => {
        let isOk = true;
        let messages = [];
        let regex = /\s/g;
        let text = this.state.title;
        text = text.replace(regex, '');
        if (!text) {
            messages.push("Title cannot be empty");
            isOk = false;
        }
        text = this.state.text;
        text = text.replace(regex, '');
        if (text.length < 5) {
            messages.push("Your post needs to be at least 5 characters long");
            isOk = false;
        }
        this.setState({ validationMessages: messages });
        return isOk;
    }
}

PostForm.propTypes = {
    isBlocked: PropTypes.bool.isRequired,
    postCallback: PropTypes.func.isRequired,
    cancelCallback: PropTypes.func
}

export default PostForm;