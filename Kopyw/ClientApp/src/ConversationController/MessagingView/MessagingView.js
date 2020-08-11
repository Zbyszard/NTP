import React, { useState, useRef, useEffect, useContext, useLayoutEffect } from 'react';
import PropTypes from 'prop-types';
import Message from '../../components/Conversation/Message';
import Communicate from '../../components/Shared/Communicate/Communicate';
import LoadingSymbol from '../../components/Shared/LoadingSymbol/LoadingSymbol';
import AuthContext from '../../Context/AuthContext';
import MessagingContext from '../../Context/MessagingContext';
import Button from '../../components/Shared/Button/Button';
import UserLink from '../../components/Shared/UserLink/UserLink';
import ContextMenu from '../../components/Shared/ContextMenu/ContextMenu';
import css from './MessagingView.module.css';
import '../../components/Shared/Icons/css/fontello.css';
import getConversationDisplayName from '../../Shared/Functions/getConversationDisplayName';

const MessagingView = props => {
    let loading = null;
    let content = null;
    const scrollableContainerRef = useRef();
    const inputRef = useRef();
    const messagingContext = useContext(MessagingContext);
    const authContext = useContext(AuthContext);
    const baseInputHeight = 38;
    const [inputHeight, setInputHeight] = useState(38);
    const [scrolledDown, setScrolledDown] = useState(true);

    const scrollHandler = () => {
        const container = scrollableContainerRef.current;
        if (container.scrollHeight - container.scrollTop === container.clientHeight ||
            container.scrollHeight === container.clientHeight)
            setScrolledDown(true);
        else
            setScrolledDown(false);
        if (!props.conversation.reachedEnd && scrollableContainerRef.current.scrollTop === 0 &&
            !messagingContext.loadingConversationIds.includes(props.conversationId))
            messagingContext.requestMessages(props.conversationId, getOldestMessageDate());
    }

    const getOldestMessageDate = () => {
        if (props.conversation.messages.length === 0)
            return undefined;
        return new Date(Math.min.apply(null, props.conversation.messages.map(m => m.sendTime)));
    }

    const inputChangeHandler = e => {
        messagingContext.setConversationInput(props.conversationId, e.target.value);
        let height = e.target.style.height;
        e.target.style.height = "1px";
        if (e.target.scrollHeight === inputHeight)
            e.target.style.height = height;
        else
            setInputHeight(e.target.scrollHeight);
        if (scrolledDown)
            scrollableContainerRef.current.scrollTop = scrollableContainerRef.current.scrollHeight;
    }

    const keyDownHandler = e => {
        if (e.key === "Enter" && !e.getModifierState("Shift")) {
            e.preventDefault();
            sendMessage();
        }
    }

    const sendMessage = () => {
        const text = props.conversation.inputValue;
        if (text.trim() === "")
            return;
        const msg = {
            conversationId: props.conversationId,
            text: text
        };
        messagingContext.sendMessage(msg);
        messagingContext.setConversationInput(props.conversationId, "");
        setInputHeight(baseInputHeight);
    }

    const scrollDown = () => {
        const container = scrollableContainerRef.current;
        container.scrollTop = container.scrollHeight;
        setScrolledDown(true);
    }

    useEffect(() => {
        inputRef.current.focus();
        let pos = props.conversation.inputValue.length;
        inputRef.current.selectionStart = inputRef.current.selectionEnd = pos;
        if (!props.conversation.loadedAny)
            messagingContext.requestMessages(props.conversationId, getOldestMessageDate());
        scrollableContainerRef.current.addEventListener("scroll", scrollHandler);
        return () => scrollableContainerRef.current.removeEventListener("scroll", scrollHandler);
    }, []);

    useLayoutEffect(() => {
        const container = scrollableContainerRef.current;
        container.scrollTo(0, container.scrollHeight);
    }, []);

    useLayoutEffect(() => {
        const container = scrollableContainerRef.current;
        if (scrolledDown) {
            scrollDown();
        }
    }, [inputHeight, props.conversation.messages.length]);

    if (props.isLoading) {
        loading = <Communicate><LoadingSymbol /></Communicate>;
    }
    if (!props.isLoading && !props.conversation.messages.length) {
        content = <Communicate>This conversation is empty</Communicate>;
    }
    else
        content = props.conversation.messages.map(m =>
            <Message key={m.sendTime.getTime()}
                sender={m.sender}
                sendTime={new Date(m.sendTime)}>
                {m.text}
            </Message>
        );

    const inputHeightpx = inputHeight + "px";
    let convDisplayName = getConversationDisplayName(props.conversation, authContext.userName);
    if (!props.conversation.isGroup)
        convDisplayName = <UserLink user={convDisplayName} />;
    return (
        <>
            <div className={css.messageTopBar}>
                <Button onClick={messagingContext.exitConversation}>
                    Back
                </Button>
                <span className={css.toRight}>
                    {convDisplayName}
                </span>
                {/* <ContextMenu>{[]}</ContextMenu> */}
            </div>
            <div className={css.outerContainer} >
                <div className={css.visibleMessages}
                    ref={scrollableContainerRef}>
                    <div className={css.messageContainer}>
                        {loading}
                        {content}
                    </div>
                </div>
                <div className={css.inputContainer}>
                    <textarea className={css.input}
                        ref={inputRef}
                        style={{ height: inputHeightpx }}
                        placeholder="Message"
                        value={props.conversation.inputValue}
                        onKeyDown={keyDownHandler}
                        onChange={inputChangeHandler} />
                    <div className={css.send} onClick={sendMessage}>
                        <i className="icon-up-big" />
                    </div>
                </div>
            </div>
        </>
    );
}

MessagingView.propTypes = {
    conversationId: PropTypes.number.isRequired,
    conversation: PropTypes.object.isRequired,
    isLoading: PropTypes.bool
}

export default MessagingView;