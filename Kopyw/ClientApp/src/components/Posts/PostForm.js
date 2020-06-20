import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService';
import axios from 'axios';
import classes from './PostForm.module.css';

class PostForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            title: "",
            text: "",
            validationMessages: []
        }
    }

    render() {
        let messages = !this.state.validationMessages.length ? null :
            this.state.validationMessages.map(msg => <li key={msg}>{msg}</li>);
        return (
            <div className={classes.container}>
                <form onSubmit={this.submitHandler}>
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
                    <input type="submit" value="Send" />
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
            [name]: value
        });
        this.setState({ validationMessages: [] });
    }

    submitHandler = async e => {
        e.preventDefault();
        if (!this.validateFields())
            return;
        await this.sendNewPost();
    }

    sendNewPost = async () => {
        const newPost = {
            Title: this.state.title,
            Text: this.state.text
        };
        let token = await authService.getAccessToken();
        let response = await axios.post("/post", newPost, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        console.log(response);
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
            messages.push("Your text needs to be at least 5 characters long");
            isOk = false;
        }
        this.setState({ validationMessages: messages });
        return isOk;
    }
}

export default PostForm;