function MiniLoaderPage({ type = "warning", size = 100 }) {
  return (
    <div style={{ position: "absolute", top: "15px", right: "15px" }}>
      <div
        className={`spinner-border text-${type}`}
        style={{ scale: `${size}%` }}
      >
        {" "}
      </div>
    </div>
  );
}

export default MiniLoaderPage;
