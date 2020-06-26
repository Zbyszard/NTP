import React, { Component } from 'react';
import axios from 'axios';
import classes from './PostForm.module.css';

class PostForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            title: "",
            text: "",
            isBlocked: false,
            validationMessages: []
        }
        this.blockTimer = null;
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

    componentWillUnmount = () => {
        clearTimeout(this.blockTimer);
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
        e.preventDefault();
        if(this.state.isBlocked)
            return;
        if (!this.validateFields())
            return;
        this.sendNewPost();
    }

    sendNewPost = () => {
        const newPost = {
            Title: this.state.title,
            Text: this.state.text
        };
        this.setState({ isBlocked: true });
        axios.post("/post", newPost)
            .then(r => {
                const post = r.data;
                this.setState({ title: "", text: "" });
                this.props.onPost(post);
            })
            .finally(() => {
                this.blockTimer = setTimeout(() => this.setState({ isBlocked: false }), 1000);
            });

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

export default PostForm;