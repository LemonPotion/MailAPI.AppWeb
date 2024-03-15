// Rectangle.jsx
import React from 'react';
import '../styles/Rectangle.css';

const Rectangle = ({ children }) => {
  return (
    <div className="rectangle">
      {children}
    </div>
  );
};

export default Rectangle;
