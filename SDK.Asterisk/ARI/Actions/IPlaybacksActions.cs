/*
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
	
	public interface IPlaybacksActions
	{
		/// <summary>
		/// Get a playback's details.. 
		/// </summary>
		/// <param name="playbackId">Playback's id</param>
		Playback Get(string playbackId);
		/// <summary>
		/// Stop a playback.. 
		/// </summary>
		/// <param name="playbackId">Playback's id</param>
		void Stop(string playbackId);
		/// <summary>
		/// Control a playback.. 
		/// </summary>
		/// <param name="playbackId">Playback's id</param>
		/// <param name="operation">Operation to perform on the playback.</param>
		void Control(string playbackId, string operation);

		/// <summary>
		/// Get a playback's details.. 
		/// </summary>
		/// <param name="playbackId">Playback's id</param>
		Task<Playback> GetAsync(string playbackId);
		/// <summary>
		/// Stop a playback.. 
		/// </summary>
		/// <param name="playbackId">Playback's id</param>
		Task StopAsync(string playbackId);
		/// <summary>
		/// Control a playback.. 
		/// </summary>
		/// <param name="playbackId">Playback's id</param>
		/// <param name="operation">Operation to perform on the playback.</param>
		Task ControlAsync(string playbackId, string operation);
	}
}
