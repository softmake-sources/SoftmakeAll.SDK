﻿/*
	AsterNET ARI Framework
	Automatically generated file @ 31/08/2020 12:42:41
*/
using System;
using System.Collections.Generic;
using SoftmakeAll.SDK.Asterisk.ARI.Models;
using SoftmakeAll.SDK.Asterisk.ARI;
using System.Threading.Tasks;

namespace SoftmakeAll.SDK.Asterisk.ARI.Actions
{
	
	public interface IMailboxesActions
	{
		/// <summary>
		/// List all mailboxes.. 
		/// </summary>
		List<Mailbox> List();
		/// <summary>
		/// Retrieve the current state of a mailbox.. 
		/// </summary>
		/// <param name="mailboxName">Name of the mailbox</param>
		Mailbox Get(string mailboxName);
		/// <summary>
		/// Change the state of a mailbox. (Note - implicitly creates the mailbox).. 
		/// </summary>
		/// <param name="mailboxName">Name of the mailbox</param>
		/// <param name="oldMessages">Count of old messages in the mailbox</param>
		/// <param name="newMessages">Count of new messages in the mailbox</param>
		void Update(string mailboxName, int oldMessages, int newMessages);
		/// <summary>
		/// Destroy a mailbox.. 
		/// </summary>
		/// <param name="mailboxName">Name of the mailbox</param>
		void Delete(string mailboxName);

		/// <summary>
		/// List all mailboxes.. 
		/// </summary>
		Task<List<Mailbox>> ListAsync();
		/// <summary>
		/// Retrieve the current state of a mailbox.. 
		/// </summary>
		/// <param name="mailboxName">Name of the mailbox</param>
		Task<Mailbox> GetAsync(string mailboxName);
		/// <summary>
		/// Change the state of a mailbox. (Note - implicitly creates the mailbox).. 
		/// </summary>
		/// <param name="mailboxName">Name of the mailbox</param>
		/// <param name="oldMessages">Count of old messages in the mailbox</param>
		/// <param name="newMessages">Count of new messages in the mailbox</param>
		Task UpdateAsync(string mailboxName, int oldMessages, int newMessages);
		/// <summary>
		/// Destroy a mailbox.. 
		/// </summary>
		/// <param name="mailboxName">Name of the mailbox</param>
		Task DeleteAsync(string mailboxName);
	}
}
