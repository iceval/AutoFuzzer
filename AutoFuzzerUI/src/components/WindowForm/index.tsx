import { FC, ReactNode } from 'react';
import './styles.scss';

interface WindowFormProps {
  className: string;
  title: string;
  children: ReactNode;
}

const WindowForm: FC<WindowFormProps> = ({ className, title, children }) => {
  return (
    <div className="window-form">
      <div className="window-form__header">{title}</div>
      <div className={`window-form__content ${className}`}>{children}</div>
    </div>
  );
};

export default WindowForm;
