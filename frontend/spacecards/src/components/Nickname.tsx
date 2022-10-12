import React from 'react';

function Nickname(props: { nickname: string }) {
  return (
    <div className='nickname-container'>
      <h1 className='header-nickname-text'>
        <a>{props.nickname}</a>
      </h1>
    </div>
  );
}

export default Nickname;
