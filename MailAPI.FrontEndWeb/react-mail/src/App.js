// App.js
import React from 'react';
import Rectangle from './components/Rectangle';
import AccentButton from './components/AccentButton';
import InputBox from './components/InputBox';

function App() {
  return (
    <div className="App">
      <Rectangle>
        <AccentButton />
        <InputBox />
      </Rectangle>
    </div>
  );
}

export default App;
