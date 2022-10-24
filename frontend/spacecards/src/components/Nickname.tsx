import React from 'react';
interface NicknameProps {
  nickname: string;
}

function Nickname(props: NicknameProps) {
  return (
    <div className='nickname-container'>
      <h1 className='header-nickname-text'>
        <a>{props.nickname}</a>
      </h1>
    </div>
  );
}

export default Nickname;
