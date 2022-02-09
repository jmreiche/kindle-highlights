import React, { Component } from "react";

export class UploadFile extends Component {
  constructor(props) {
    super(props);
    this.state = { file: "", msg: "", books: [], selectedBook: "" };
  }

  onFileChange = (event) => {
    this.setState({
      file: event.target.files[0],
    });
  };

  uploadFileData = (event) => {
    event.preventDefault();
    this.setState({ msg: "" });

    let data = new FormData();
    data.append("file", this.state.file);

    fetch("fileupload", {
      method: "POST",
      body: data,
    })
      .then(async (response) => {
        if (!response.ok) {
          console.log(
            "Response from addTrainingProgram not OK: ",
            response.status
          );
          throw new Error(response.status);
        }

        const json = await response.json();

        this.setState({ books: Object.entries(json.content) });

        return json;
      })
      .catch((err) => {
        this.setState({ error: err });
      });
  };

  handleChange = (event) => {
    this.setState({
      highlights: Object.entries(this.state.books[event.target.value][1]),
    });
  };

  render() {
    let books = this.state.books;
    let optionItems = books.map((book, index) => (
      <option key={index} value={index}>
        {book[0]}
      </option>
    ));
    return (
      <div id="container">
        <h1>Upload your My Clippings.txt file</h1>
        <ol>
          <li>Plug in your Kindle and the file will usually be found under /Kindle/documents/</li>
          <li>Press the "Browse" button below find the file and then press "Upload"</li>
          <li>Select the book or pdf from the dropdown to see all highlights</li>
        </ol>
        <h4>{this.state.msg}</h4>
        <input onChange={this.onFileChange} type="file"></input>
        <button disabled={!this.state.file} onClick={this.uploadFileData}>
          Upload
        </button>
        <br></br>
        <select onChange={this.handleChange}>{optionItems}</select>
        {this.state.highlights
          ? this.state.highlights.map((highlight, index) => (
              <div key={index}>
                {/* <h5>Location: {highlight[0]}</h5> */}
                <blockquote>{highlight[1]}</blockquote>
              </div>
            ))
          : null}
      </div>
    );
  }
} 
